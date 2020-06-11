CREATE PROCEDURE [dbo].[SearchShloka] (
    @ShlokatoSearch NVARCHAR(MAX),
    	@page_index int = 1,
	@page_Size int = 10,
	@get_count bit 
   -- @Book_name NVARCHAR(MAX) OUTPUT,
	--@Sthana_name NVARCHAR(MAX) OUTPUT,
	--@Adhyaya_name NVARCHAR(MAX) OUTPUT,
	--@Shloka_out NVARCHAR(MAX) OUTPUT
) AS
BEGIN

   	IF (ISNULL(@page_index,'')='')
	  
		BEGIN
		SET @page_index=1
		END
    IF (ISNULL(@page_Size,'')='')
	  
		BEGIN
		SET @page_Size=10
		END
		
	if(@get_count=1)
		BEGIN
		  
		  SELECT count(*) FROM (
		  select distinct bk.name, stha.sthana_name,adhy.adhyaya_name, sl.shloka from shloka as sl with(NOLOCK) 
		  inner join adhikarana as adhi with(NOLOCK) on sl.adhikarana_ref=adhi.adhikarana_id 
		  inner join adhyaya as adhy with(NOLOCK) on adhy.adhyaya_id=adhi.adhyaya_ref
		  inner join sthana as stha with(NOLOCK) on stha.sthana_id=adhy.sthana_ref
		  inner join book as bk with(NOLOCK) on bk.book_id=stha.book_ref where sl.shloka like N'%'+@ShlokatoSearch+'%') U
		
		END
		
		
	ELSE
		BEGIN
		
		SELECT X.* FROM (
		SELECT W.*, row_number() OVER ( ORDER BY W.shloka ) AS row_num FROM (
		SELECT U.* FROM (
		  select distinct bk.name, stha.sthana_name,adhy.adhyaya_name, sl.shloka from shloka as sl with(NOLOCK) 
		  inner join adhikarana as adhi with(NOLOCK) on sl.adhikarana_ref=adhi.adhikarana_id 
		  inner join adhyaya as adhy with(NOLOCK) on adhy.adhyaya_id=adhi.adhyaya_ref
		  inner join sthana as stha with(NOLOCK) on stha.sthana_id=adhy.sthana_ref
		  inner join book as bk with(NOLOCK) on bk.book_id=stha.book_ref where  sl.shloka like N'%'+@ShlokatoSearch+'%') U )W)X
		  
	 WHERE row_num BETWEEN  CONVERT(varchar,((@page_index - 1) * @page_Size + 1 )) AND CONVERT(varchar, @page_index * @page_Size) 
		  
 		 END
  
	END;
GO

