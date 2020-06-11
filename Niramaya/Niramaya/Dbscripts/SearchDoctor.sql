/*Created by : KalT*/

CREATE PROCEDURE [dbo].[SearchDoctor] (
    
    @Input_text VARCHAR(MAX)=NULL,
    @get_count BIT,
    @page_index int = 1,
	@page_Size int = 10
) AS
BEGIN



-- Handle indexing if incoming value is blank
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

select count(*) from Doc_Detail WHERE ispublish=1 
  AND (
        doc_Fname LIKE '%'+@Input_text+'%' 
        OR doc_Lname LIKE '%'+@Input_text+'%' 
        OR doc_Mname LIKE '%'+@Input_text+'%'
        OR doc_GradDegree LIKE  '%'+@Input_text+'%'
        OR doc_PostGrad LIKE  '%'+@Input_text+'%'
        OR doc_Phone LIKE  '%'+@Input_text+'%'
        OR doc_Email LIKE  '%'+@Input_text+'%'
        OR Clinic_address1 LIKE  '%'+@Input_text+'%'
        OR clinic_address2 LIKE  '%'+@Input_text+'%' 
        OR clinic_city LIKE  '%'+@Input_text+'%' 
        OR clinic_state LIKE  '%'+@Input_text+'%' 
        OR clinic_pincode LIKE  '%'+@Input_text+'%' 
       	OR doc_Services LIKE  '%'+@Input_text+'%' 
        
        )
END

ELSE 
BEGIN


SELECT
A.* FROM 
 (
			select userid, username, doc_Fname, doc_Mname, doc_Lname, doc_Gender, doc_DOB,
		 	doc_GradDegree, doc_PostGrad, doc_Phone, doc_Email, clinic_address1,
		 	clinic_address2, clinic_city, clinic_state, clinic_pincode, clinic_phone, 
		  	doc_Services, row_number() OVER (ORDER BY doc_email) AS row_num  from Doc_Detail 
		  	WHERE ispublish=1 
		 	AND (
			        doc_Fname LIKE '%'+@Input_text+'%' 
			        OR doc_Lname LIKE '%'+@Input_text+'%' 
			        OR doc_Mname LIKE '%'+@Input_text+'%'
			        OR doc_GradDegree LIKE  '%'+@Input_text+'%'
			        OR doc_PostGrad LIKE  '%'+@Input_text+'%'
			        OR doc_Phone LIKE  '%'+@Input_text+'%'
			        OR doc_Email LIKE  '%'+@Input_text+'%'
			        OR Clinic_address1 LIKE  '%'+@Input_text+'%'
			        OR clinic_address2 LIKE  '%'+@Input_text+'%' 
			        OR clinic_city LIKE  '%'+@Input_text+'%' 
			        OR clinic_state LIKE  '%'+@Input_text+'%' 
			        OR clinic_pincode LIKE  '%'+@Input_text+'%' 
			       	OR doc_Services LIKE  '%'+@Input_text+'%' 
			        
	        	  )
      )A WHERE row_num BETWEEN  CONVERT(varchar,((@page_index - 1) * @page_Size + 1 )) AND CONVERT(varchar, @page_index * @page_Size) 
END

/*
DECLARE @Query VARCHAR(MAX);
SET @Query=

'select userid, username, doc_Fname, doc_Mname, doc_Lname, doc_Gender, doc_DOB,
 doc_GradDegree, doc_PostGrad, doc_Phone, doc_Email, clinic_address1,
  clinic_address2, clinic_city, clinic_state, clinic_pincode, clinic_phone, 
  doc_Services, ispublish  from Doc_Detail WHERE ispublish=1 '+
  'AND (doc_Fname LIKE ''%'+@Input_text+'%'' OR doc_Mname LIKE ''%'+@Input_text+'%'' OR doc_Lname LIKE ''%'+@Input_text+'%''
   )'
--EXECUTE @Query
--PRINT @Query
EXEC(@Query)
*/
	END
GO

