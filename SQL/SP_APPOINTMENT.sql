USE IF4101_proyecto3_B95212_B97452;

CREATE PROCEDURE ADMINISTRATOR.sp_VALIDATE_DOCTOR_LOG_IN
	@param_ID_CARD     VARCHAR(32),
	@param_DOCTOR_CODE VARCHAR(32),
	@param_PASSWORD    VARCHAR(32)
AS
BEGIN
	IF NOT EXISTS (SELECT [ID_CARD], [DOCTOR_CODE], [PASSWORD] FROM [ADMINISTRATOR].[tb_DOCTOR] WHERE [ID_CARD] = @param_ID_CARD 
	AND [DOCTOR_CODE] = @param_DOCTOR_CODE AND [PASSWORD] = @param_PASSWORD)
		BEGIN
			RETURN -1;
		END
	ELSE
		BEGIN 
			RETURN 1;
		END
END
GO
-----------------------------------------------------------------------

CREATE PROCEDURE ADMINISTRATOR.sp_GET_SPECIALTIES_TYPES
AS
BEGIN
	SELECT [SPECIALTY_TYPE] FROM [ADMINISTRATOR].[tb_SPECIALTY]
END
GO

-----------------------------------------------------------------------

CREATE PROCEDURE ADMINISTRATOR.sp_GET_MEDICALS_CENTERS_NAMES
AS
BEGIN
	SELECT [MEDICAL_CENTER_NAME]  FROM [ADMINISTRATOR].[tb_MEDICAL_CENTER]
END
GO
-----------------------------------------------------------------------

CREATE PROCEDURE ADMINISTRATOR.sp_REGISTER_APPOINTMENT
	@param_ID_CARD VARCHAR(32),
	@param_MEDICAL_CENTER_NAME VARCHAR(32),
	@param_DATE DATETIME,
	@param_SPECIALTY_TYPE VARCHAR(100)
AS
BEGIN
	DECLARE @local_PATIENT_ID INT = (SELECT [PATIENT_ID] FROM [PATIENT].[tb_PATIENT] WHERE [ID_CARD] = @param_ID_CARD)
	DECLARE @local_MEDICAL_CENTER_ID INT = (SELECT [MEDICAL_CENTER_ID] FROM [ADMINISTRATOR].[tb_MEDICAL_CENTER] WHERE [MEDICAL_CENTER_NAME] = @param_MEDICAL_CENTER_NAME)
	DECLARE @local_SPECIALTY_ID INT = (SELECT [SPECIALTY_ID] FROM [ADMINISTRATOR].[tb_SPECIALTY] WHERE [SPECIALTY_TYPE] = @param_SPECIALTY_TYPE)


	IF NOT EXISTS (SELECT [APPOINTMENT_ID] FROM [ADMINISTRATOR].[tb_APPOINTMENT] 
	WHERE [DATE] = @param_DATE AND [PATIENT_ID] = @local_PATIENT_ID)
		BEGIN
			INSERT INTO  [ADMINISTRATOR].[tb_APPOINTMENT]([PATIENT_ID], [DATE], [MEDICAL_CENTER_ID], [SPECIALTY_ID])
			VALUES(@local_PATIENT_ID, @param_DATE, @local_MEDICAL_CENTER_ID, @local_SPECIALTY_ID)
			RETURN 1
		END
	ELSE
		BEGIN
			RETURN -1
		END
END

-----------------------------------------------------------------------

CREATE PROCEDURE ADMINISTRATOR.sp_GET_APPOINTMENTS_BY_CARD
	@param_ID_CARD VARCHAR(32)
AS
BEGIN
		DECLARE @local_PATIENT_ID INT = (SELECT [PATIENT_ID] FROM [PATIENT].[tb_PATIENT] WHERE [ID_CARD] = @param_ID_CARD)

	SELECT 
END