USE IF4101_proyecto3_B95212_B97452;

CREATE PROCEDURE ADMINISTRATOR.sp_VALIDATE_DOCTOR_LOG_IN
	@param_ID_CARD     VARCHAR(32),
	@param_DOCTOR_CODE INT,
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