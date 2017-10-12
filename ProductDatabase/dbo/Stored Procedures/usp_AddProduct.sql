CREATE PROCEDURE [dbo].[usp_AddProduct]
	@Code			VARCHAR (50),
    @Name			NVARCHAR (50),
    @Description	NVARCHAR (MAX),
    @Cost			MONEY,
    @ListPrice		MONEY,
    @CategoryId		INT,
    @SupplierId		INT,
	@ReleaseDate	DATE,
	@Id				INT OUTPUT
AS
	SET NOCOUNT ON;

	DECLARE @Date datetime;
	EXEC [dbo].[usp_GetDate] @Date OUTPUT;

	IF NOT EXISTS (SELECT * FROM  [dbo].[Category] c WHERE c.Id = @CategoryId)
	BEGIN
		RAISERROR ('Category not found', 16, 1);
		RETURN 0;
	END

	IF NOT EXISTS (SELECT * FROM  [dbo].[Supplier] s WHERE s.Id = @SupplierId)
	BEGIN
		RAISERROR ('Supplier not found', 16, 1);
		RETURN 0;
	END

	INSERT [dbo].[Product]
	([Code], [Name], [Description], [Cost], [ListPrice], [CategoryId], [SupplierId], [ReleaseDate], [CreatedOn])
	VALUES (@Code, @Name, @Description, @Cost, @ListPrice, @CategoryId, @SupplierId, @ReleaseDate, @Date);

	SET @Id = SCOPE_IDENTITY();

	RETURN 0;