CREATE PROCEDURE [dbo].[usp_GetDate]
	@Date datetime output
AS
BEGIN
	SET NOCOUNT ON;
	SET @Date = GETDATE();
	RETURN;    	
END