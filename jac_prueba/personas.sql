  DROP TABLE jac_persona;
-- Tabla que almacena los datos
CREATE  TABLE jac_persona ( 
	nIdPersona           INT UNSIGNED   NOT NULL AUTO_INCREMENT   PRIMARY KEY,
	sNombre              NVARCHAR(255)    NOT NULL   ,
	sApellido            NVARCHAR(255)    NOT NULL   ,
	sDNI                 NVARCHAR(50)    NOT NULL   ,
	dFechaNacimiento     DATE    NOT NULL   ,
	sEmail               NVARCHAR(255)    NOT NULL   ,
	bActivo              BIT  DEFAULT 1  NOT NULL   
 );

ALTER TABLE jac_persona MODIFY sNombre NVARCHAR(255)  NOT NULL   COMMENT 'Nombres de la persona';
ALTER TABLE jac_persona MODIFY sApellido NVARCHAR(255)  NOT NULL   COMMENT 'Apellidos de la persona';
ALTER TABLE jac_persona MODIFY sDNI NVARCHAR(50)  NOT NULL   COMMENT 'Carnet o identificador legal de la persona';
ALTER TABLE jac_persona MODIFY sEmail NVARCHAR(255)  NOT NULL   COMMENT 'Correo electronico de la persona';
ALTER TABLE jac_persona MODIFY bActivo BIT  NOT NULL DEFAULT 1  COMMENT 'inidica si el registro está activo';


DROP PROCEDURE IF EXISTS sp_jac_persona;
DELIMITER ;;
CREATE PROCEDURE sp_jac_persona (
  IN Ck_IdOpcion            INT, -- 1 Insert, 2,Update, 3 delete, 4, select
  IN Ck_IdPersona           INT, -- 
  IN Ck_Nombre              NVARCHAR(255),
  IN Ck_Apellido            NVARCHAR(255),
  IN Ck_DNI                 NVARCHAR(50),
  IN Ck_FechaNacimiento     DATE,
  IN Ck_Email               NVARCHAR(255)
) BEGIN
/******************************************************************************
* Nombre..........: sp_jac_persona
* Propósito.......: Controla el CRUD de la tabla	jac_persona		
* Autor...........: Joaquín A. Castro Valle
* Empresa.........: 
* F. Creación.....: 12-12-2024
* Modificaciones
* Fecha/Autor.: 
* Modificación..: 
* Ejemplo:        CALL sp_jac_persona('2023-03-31','2023-03-31',-1,'','','');
*****************************************************************************/

IF Ck_IdOpcion = 1 THEN -- Inserta el registro
    SET Ck_IdPersona = -1;
    INSERT INTO jac_persona(sNombre   ,sApellido   ,sDNI   ,dFechaNacimiento   ,sEmail)
    VALUE                  (Ck_Nombre ,Ck_Apellido ,Ck_DNI ,Ck_FechaNacimiento ,Ck_Email);

    SELECT LAST_INSERT_ID() into Ck_IdPersona;

    -- Retorna el id de la persona para saber que se creó el registro
    SELECT 	Ck_IdPersona as idPersona; 
END IF;

IF (Ck_IdOpcion = 2) THEN -- Actualiza

    UPDATE jac_persona SET
        sNombre          = Ck_Nombre,
        sApellido        = Ck_Apellido,
        sDNI             = Ck_DNI,
        dFechaNacimiento = Ck_FechaNacimiento,
        sEmail           = Ck_Email         
    WHERE nIdPersona = Ck_IdPersona and bActivo = 1;

    SELECT ROW_COUNT() AS filasAfectadas;

END IF;

IF (Ck_IdOpcion = 3) THEN -- Elimina

    UPDATE jac_persona SET    
      bActivo = 0            
    WHERE nIdPersona = Ck_IdPersona;

    SELECT ROW_COUNT() AS filasAfectadas;

END IF;

IF (Ck_IdOpcion = 4) THEN -- select 

    SELECT 
        nIdPersona
        ,sNombre
        ,sApellido
        ,sDNI
        ,dFechaNacimiento
        ,sEmail
    FROM jac_persona
    WHERE nIdPersona = CASE WHEN Ck_IdPersona = -1 THEN nIdPersona ELSE Ck_IdPersona END -- Si no es -1 trae el registro especifico, de lo contrario trae todos los registros
    AND bActivo = 1;

END IF;

END;;
 
