CREATE VIEW dbo.uw_Product
AS
SELECT	p.Id,
		p.Code, 
		p.Name, 
		c.Name AS Category

FROM		dbo.Product p
INNER JOIN	dbo.Category c ON p.CategoryId = c.Id
GO
