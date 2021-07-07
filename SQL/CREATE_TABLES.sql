GO
CREATE SCHEMA ADMINISTRATOR;
CREATE SCHEMA PATIENT;

GO

CREATE TABLE ADMINISTRATOR.tb_DOCTOR
(
DOCTOR_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
ID_CARD VARCHAR(32) NOT NULL, 
DOCTOR_CODE VARCHAR(32)NOT NULL, 
PASSWORD VARCHAR(32) NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL
);

CREATE TABLE ADMINISTRATOR.tb_BLOOD
(
BLOOD_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
BLOOD_TYPE VARCHAR(32) NOT NULL,
IS_DELETED BIT DEFAULT 0  NULL
);

CREATE TABLE PATIENT.tb_PHONE(
PHONE_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
PHONE_NUMBER VARCHAR(32) NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL
);

CREATE TABLE PATIENT.tb_PATIENT
(
PATIENT_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
ID_CARD VARCHAR(32) NOT NULL,
NAME VARCHAR(32) NOT NULL,
LAST_NAME VARCHAR(100) NOT NULL,
PASSWORD VARCHAR(200) NOT NULL,
AGE INT NOT NULL,
BLOOD_ID INT NOT NULL,
CIVIL_STATUS VARCHAR(32) NOT NULL,
ADDRESS VARCHAR(32) NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL
CONSTRAINT fk_blood_id_type FOREIGN KEY (BLOOD_ID) REFERENCES ADMINISTRATOR.tb_BLOOD(BLOOD_ID)
);

CREATE TABLE ADMINISTRATOR.tb_MEDICINE
(
MEDICINE_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
MEDICINE_TYPE VARCHAR(32) NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL
);

CREATE TABLE ADMINISTRATOR.tb_VACCINE
(
VACCINE_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
VACCINE_TYPE VARCHAR(32) NOT NULL,
DESCRIPTION VARCHAR(200) NULL,
IS_DELETED BIT DEFAULT 0 NULL
);

CREATE TABLE ADMINISTRATOR.tb_ALLERGY
(
ALLERGY_ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
ALLERGY_TYPE VARCHAR(32) NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL
);

CREATE TABLE ADMINISTRATOR.tb_MEDICAL_CENTER
(
MEDICAL_CENTER_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
MEDICAL_CENTER_NAME VARCHAR(200) NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL
);

CREATE TABLE ADMINISTRATOR.tb_SPECIALTY
(
SPECIALTY_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
SPECIALTY_TYPE VARCHAR(100) NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL
);

CREATE TABLE ADMINISTRATOR.tb_APPOINTMENT
(
APPOINTMENT_ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
PATIENT_ID INT NOT NULL,
DATE DATETIME NOT NULL,
MEDICAL_CENTER_ID INT NOT NULL,
DESCRIPTION VARCHAR(200) NULL,
SPECIALTY_ID INT NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL,
CONSTRAINT fk_patient_id_appointment FOREIGN KEY (PATIENT_ID) REFERENCES PATIENT.tb_PATIENT(PATIENT_ID),
CONSTRAINT fk_medical_center_id_appointment FOREIGN KEY (MEDICAL_CENTER_ID) REFERENCES ADMINISTRATOR.tb_MEDICAL_CENTER(MEDICAL_CENTER_ID),
CONSTRAINT fk_specialty_type_appointment FOREIGN KEY (SPECIALTY_ID) REFERENCES ADMINISTRATOR.tb_SPECIALTY(SPECIALTY_ID)
);

CREATE TABLE PATIENT.tb_VACCINE_PATIENT
(
PATIENT_ID INT NOT NULL,
VACCINE_ID INT NOT NULL,
DESCRIPTION VARCHAR(200) NULL,
LATTEST_VACCINE_DATE DATE,
NEXT_VACCINE_DATE DATE,
IS_DELETED BIT DEFAULT 0 NULL
CONSTRAINT pk_vaccine_PATIENT PRIMARY KEY (PATIENT_ID,VACCINE_ID),
CONSTRAINT fk_patient_id_vaccine_patient FOREIGN KEY (PATIENT_ID) REFERENCES PATIENT.tb_PATIENT(PATIENT_ID),
CONSTRAINT fk_vaccine_id_vaccine_patient FOREIGN KEY (VACCINE_ID)REFERENCES ADMINISTRATOR.tb_VACCINE(VACCINE_ID)
);

CREATE TABLE PATIENT.tb_PHONE_PATIENT
(
PATIENT_ID INT NOT NULL,
PHONE_ID INT NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL,
CONSTRAINT pk_patient_phone PRIMARY KEY (PATIENT_ID, PHONE_ID),
CONSTRAINT fk_patient_id_phone FOREIGN KEY (PATIENT_ID) REFERENCES PATIENT.tb_PATIENT(PATIENT_ID),
CONSTRAINT fk_phone_id_patient FOREIGN KEY (PHONE_ID) REFERENCES PATIENT.tb_PHONE(PHONE_ID)
);

CREATE TABLE PATIENT.tb_ALLERGY_PATIENT
(
PATIENT_ID INT NOT NULL,
ALLERGY_ID INT NOT NULL,
DIAGNOSTIC_DATE DATE NOT NULL,
DESCRIPTION VARCHAR(200) NULL,
IS_DELETED BIT DEFAULT 0 NULL,
CONSTRAINT pk_allergy_patient PRIMARY KEY (PATIENT_ID,ALLERGY_ID),
CONSTRAINT fk_patient_id_allergy_patient FOREIGN KEY (PATIENT_ID) REFERENCES PATIENT.tb_PATIENT(PATIENT_ID),
CONSTRAINT fk_allergy_id_allergy_patient FOREIGN KEY (ALLERGY_ID) REFERENCES ADMINISTRATOR.tb_ALLERGY(ALLERGY_ID)
);

CREATE TABLE PATIENT.tb_MEDICINE_ALLERGY_PATIENT
(
MEDICINE_ID INT NOT NULL,
PATIENT_ID INT NOT NULL,
ALLERGY_ID INT NOT NULL,
IS_DELETED BIT DEFAULT 0 NULL
CONSTRAINT pk_medicine_allergy_patient PRIMARY KEY (MEDICINE_ID,PATIENT_ID,ALLERGY_ID),
CONSTRAINT fk_patient_id_medicine_allergy_patient FOREIGN KEY (PATIENT_ID) REFERENCES PATIENT.tb_PATIENT(PATIENT_ID),
CONSTRAINT fk_allergy_id_medicine_allergy_patient FOREIGN KEY (ALLERGY_ID) REFERENCES ADMINISTRATOR.tb_ALLERGY(ALLERGY_ID),
CONSTRAINT fk_medicine_id_medicine_allergy_patient FOREIGN KEY (MEDICINE_ID) REFERENCES ADMINISTRATOR.tb_MEDICINE(MEDICINE_ID)
);


