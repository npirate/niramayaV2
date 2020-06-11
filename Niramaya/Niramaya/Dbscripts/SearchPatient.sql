/*Created by KalT*/


CREATE PROCEDURE [dbo].[SearchPatient] (
	@Pat_UID  uniqueidentifier =NULL,
    @Pat_mobile VARCHAR(10)=NULL,
    @Pat_LastName VARCHAR(25)=NULL,
	@Pat_FirstName VARCHAR(25)=NULL,
	@Pat_MiddleName VARCHAR(25)=NULL,
	@Pat_DOB Date =NULL,
	@Pat_PinCode VARCHAR(25)=NULL,
	@page_index int = 1,
	@page_Size int = 10,
	@get_count bit 

) AS
BEGIN


DECLARE @TotalRecords bigint =0

-- Handle indexing if incoming value is blank
	IF (ISNULL(@page_index,'')='')
	  
		BEGIN
		SET @page_index=1
		END
    IF (ISNULL(@page_Size,'')='')
	  
		BEGIN
		SET @page_Size=10
		END
		


DECLARE @QUERY VARCHAR(MAX);

if(@get_count=1)
BEGIN

		SET @QUERY='select count(*) from (Select PatientUID, Fname,Lname,Gender,DOB, Mob,row_number() over ( order by patientuid asc) as row_num  from Patient_Detail where 1=1 '
		IF(@Pat_mobile IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND Mob='''+@Pat_mobile+''' '
		ENd

		IF(@Pat_LastName IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND Lname like ''%'+@Pat_LastName+'%'' '
		ENd

		IF(@Pat_FirstName IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND Fname like ''%'+@Pat_FirstName+'%'' '
		ENd

		IF(@Pat_MiddleName IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND Mname like ''%'+@Pat_MiddleName+'%'' '
		ENd

		IF(@Pat_DOB IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND DOB = '''+Cast(@Pat_DOB as varchar(10))+''''
		ENd

		IF(@Pat_PinCode IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND pincode like ''%'+@Pat_PinCode+'%'' '
		ENd

		SET @Query=@Query+ ') as U '
		print @Query
			EXEC (@QUERY)

END
ELSE
BEGIN

		SET @QUERY='select U.* from (Select PatientUID, Fname,Lname,Gender,CONVERT( varchar, DOB , 101) as DOB , Mob,row_number() over ( order by patientuid asc) as row_num  from Patient_Detail where 1=1 '

		IF(@Pat_UID IS NOT NULL)
		BEGIN
			Set @QUERY=NULL;
			SET @QUERY= 'Select * from Patient_Detail where 1=1 AND PatientUID='''+ CAST(@Pat_UID AS NVARCHAR(MAX)) +''' '
			EXEC (@QUERY)
		RETURN;
		ENd

		IF(@Pat_mobile IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND Mob='''+@Pat_mobile+''' '
			ENd

		IF(@Pat_LastName IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND Lname like ''%'+@Pat_LastName+'%'' '
			ENd

		IF(@Pat_FirstName IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND Fname like ''%'+@Pat_FirstName+'%'' '
			ENd

		IF(@Pat_MiddleName IS NOT NULL)
			BEGIN
			Set @QUERY = @QUERY+'AND Mname like ''%'+@Pat_MiddleName+'%'' '
			ENd

		IF(ISNULL(@Pat_DOB,'')<>'')
			BEGIN
			Set @QUERY = @QUERY+'AND DOB = '''+Cast(@Pat_DOB as varchar(10))+''' '
			ENd


		IF(@Pat_PinCode IS NOT NULL)
			BEGIN
			
			
			Set @QUERY = @QUERY+' AND pincode like ''%'+@Pat_PinCode+'%'' '
		   
			ENd

		SET @Query=@Query+ ') as U WHERE row_num BETWEEN '+  CONVERT(varchar,((@page_index - 1) * @page_Size + 1 ))+ ' AND '+ CONVERT(varchar, @page_index * @page_Size) 
		
			EXEC (@QUERY)
	END
	END;
GO

