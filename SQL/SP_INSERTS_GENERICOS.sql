GO

--------------------****BLOOD TYPES**********----------
INSERT INTO ADMINISTRATOR.tb_BLOOD (BLOOD_TYPE) VALUES ('A+'),('O-');


---------------------¨*****PATIENTS****--------------

INSERT INTO PATIENT.tb_PATIENT 
(
[ID_CARD],
[NAME],
[LAST_NAME],
[PASSWORD],
[AGE],
[BLOOD_ID],
[CIVIL_STATUS],
[ADDRESS]
)

VALUES
(
'700330510',
'Alberto',
'Cañas Alfaro',
'Tuis2021',
45,
1,
'Soltero',
'Tuis, Turrialba'
),
(
'300650532',
'Maria',
'Trejos Rojas',
'MariFonde67',
26,
2,
'Soltero',
'Cervantes, Cartago'
),
(
'201750626',
'Fabiola',
'Luna Fernandez',
'FLF23Turri',
32,
1,
'Casada',
'El Recreo, Turrialba'
);

--------------------------*******ALLERGIES*********------------------------

INSERT INTO ADMINISTRATOR.tb_ALLERGY (ALLERGY_TYPE) VALUES ('Pet'),('Insect'),('Drug'),('Food'),('Pollen')

------------------------********MEDICAL CENTER*****.-----------------------

INSERT INTO ADMINISTRATOR.tb_MEDICAL_CENTER (MEDICAL_CENTER_NAME) VALUES ('Sabana MC'),('Rojas MC'), ('CIMA Hospital'), ('San Juan de Dios Hospital')

-----------------------*******MEDICINE***********-----------------------

INSERT INTO ADMINISTRATOR.tb_MEDICINE (MEDICINE_TYPE) VALUES ('Loratadina'),('Cetirizina'),('Desloratadina'),('Fexofenadina'),('Levocetirizina'),('Clorfeniramina'),('Ketotifen')

----------------------*******SPECIALTY**********----------------------

INSERT INTO ADMINISTRATOR.tb_SPECIALTY (SPECIALTY_TYPE) VALUES ('Anesthesiology'),('Pediatrics'),('Dermatology'),('Neurology'),('Urology');

--------------------********VACCINE***********----------------

INSERT INTO ADMINISTRATOR.tb_VACCINE
(VACCINE_TYPE) 
VALUES 
('Toxoid'),
('Viral vector'),
('Inactivated'),
('Messenger RNA'),
('polysaccharide')