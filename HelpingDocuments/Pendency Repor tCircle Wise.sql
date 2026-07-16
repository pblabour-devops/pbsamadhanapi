-- Total Pending applications
with cte(LabourCircleName, lid, total) as( select cir.LabourCircleName, cir.LabourCircleId lid, count(*) total from Applications app
	inner join ApplicationActions ac
		on app.AppId = ac.ApplicationRefId
	inner join UserCircleMappings ucm
		on ac.Receiver_UserRefId = ucm.UserRefId and ucm.Version=2
	inner join LabourCircles cir
		on ucm.LabourCircleRefId = cir.LabourCircleId
	where app.ApplicationType = 6 and ac.AppActionType in (5,6) 
	group by cir.LabourCircleName, cir.LabourCircleId 
	)
	select cir.LabourCircleName,up.FirstName +' ' + up.LastName AS 'Pending With',
	(case when c.total is null Then 0 else c.total end) AS TotalPendingApplications
	from cte c
	right join LabourCircles cir
		on lid = cir.LabourCircleId
	inner join UserCircleMappings ucm
		on cir.LabourCircleId = ucm.LabourCircleRefId
	inner join UserProfileMapping upm
		on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	inner join UserProfiles up
		on upm.UserProfileRefId = up.UserProfileId
		where cir.Version=2
	order by cir.LabourCircleName



--Total Approved
with cte(LabourCircleName, lid, total) as( select cir.LabourCircleName, cir.LabourCircleId lid, count(*) total from Applications app
	inner join ApplicationActions ac
		on app.AppId = ac.ApplicationRefId
	inner join UserCircleMappings ucm
		on ac.Sender_UserRefId = ucm.UserRefId and ucm.Version=2
	inner join LabourCircles cir
		on ucm.LabourCircleRefId = cir.LabourCircleId
	where app.ApplicationType = 6 and ac.AppActionType in (200) 
	group by cir.LabourCircleName, cir.LabourCircleId 
	)
	select cir.LabourCircleName,up.FirstName +' ' + up.LastName as 'Approved By',
	(case when c.total is null Then 0 else c.total end) AS TotalApproved
	from cte c
	right join LabourCircles cir
		on lid = cir.LabourCircleId
	inner join UserCircleMappings ucm
		on cir.LabourCircleId = ucm.LabourCircleRefId
	inner join UserProfileMapping upm
		on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	inner join UserProfiles up
		on upm.UserProfileRefId = up.UserProfileId
		where cir.Version=2
	order by cir.LabourCircleName


--Total In-Onjection
with cte(LabourCircleName, lid, total) as( select cir.LabourCircleName, cir.LabourCircleId lid, count(*) total 
from Applications app
	inner join ApplicationActions ac
		on app.AppId = ac.ApplicationRefId
	inner join UserCircleMappings ucm
		on ac.Sender_UserRefId = ucm.UserRefId and ucm.Version=2
	inner join LabourCircles cir
		on ucm.LabourCircleRefId = cir.LabourCircleId
	where app.ApplicationType = 6 and ac.AppActionType in (404) 
	group by cir.LabourCircleName, cir.LabourCircleId 
	)
	select cir.LabourCircleName,up.FirstName +' ' + up.LastName as 'Objection Raised By',
	(case when c.total is null Then 0 else c.total end) AS ObjectionRaisedInApplications
	from cte c
	right join LabourCircles cir
		on lid = cir.LabourCircleId
	inner join UserCircleMappings ucm
		on cir.LabourCircleId = ucm.LabourCircleRefId
	inner join UserProfileMapping upm
		on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	inner join UserProfiles up
		on upm.UserProfileRefId = up.UserProfileId
		where cir.Version=2
	order by cir.LabourCircleName


--Total Rejected
with cte(LabourCircleName, lid, total) as( select cir.LabourCircleName, cir.LabourCircleId lid, count(*) total from Applications app
	inner join ApplicationActions ac
		on app.AppId = ac.ApplicationRefId
	inner join UserCircleMappings ucm
		on ac.Sender_UserRefId = ucm.UserRefId and ucm.Version=2
	inner join LabourCircles cir
		on ucm.LabourCircleRefId = cir.LabourCircleId
	where app.ApplicationType = 6 and ac.AppActionType in (201) 
	group by cir.LabourCircleName, cir.LabourCircleId 
	)
	select cir.LabourCircleName,up.FirstName +' ' + up.LastName as 'Application Rejected By',
	(case when c.total is null Then 0 else c.total end) AS TotalApplicationRejected
	from cte c
	right join LabourCircles cir
		on lid = cir.LabourCircleId
	inner join UserCircleMappings ucm
		on cir.LabourCircleId = ucm.LabourCircleRefId
	inner join UserProfileMapping upm
		on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	inner join UserProfiles up
		on upm.UserProfileRefId = up.UserProfileId
		where cir.Version=2
	order by cir.LabourCircleName

--Total Deemed
-- with cte(LabourCircleName, lid, total) as( select cir.LabourCircleName, cir.LabourCircleId lid, count(*) total from Applications app
	-- inner join ApplicationActions ac
		-- on app.AppId = ac.ApplicationRefId
	-- inner join UserCircleMappings ucm
		-- on ac.Sender_UserRefId = ucm.UserRefId --and ucm.Version=2
	-- inner join LabourCircles cir
		-- on ucm.LabourCircleRefId = cir.LabourCircleId
	-- where app.ApplicationType = 6 and ac.AppActionType in (206) 
	-- group by cir.LabourCircleName, cir.LabourCircleId 
	-- )
	-- select cir.LabourCircleName,up.FirstName +' ' + up.LastName as 'Deemed From Officer',
	-- (case when c.total is null Then 0 else c.total end) AS TotalApplicationDeemed
	-- from cte c
	-- right join LabourCircles cir
		-- on lid = cir.LabourCircleId
	-- inner join UserCircleMappings ucm
		-- on cir.LabourCircleId = ucm.LabourCircleRefId
	-- inner join UserProfileMapping upm
		-- on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	-- inner join UserProfiles up
		-- on upm.UserProfileRefId = up.UserProfileId
		-- --where cir.Version=2
	-- order by cir.LabourCircleName

	-- Shop deemed
	select pf.Officer_UserRefId, up.FirstName + ' ' + up.LastName as 'Officer Name', count(*) as 'TotalApplications' from Deemed_ProcessFilesLogs pf
		inner join UserProfileMapping upm
			on pf.Officer_UserRefId = upm.UserRefId
		inner join UserProfiles up
			on upm.UserProfileRefId = up.UserProfileId
		group by pf.Officer_UserRefId,up.FirstName , up.LastName

		select * from aspnetusers where id='182c6885-50ca-456b-8fa8-49b001f94c1a'





---- **************************** Factory Licence Pendency Report

-- Total Pending applications (Dealing Hand)
with cte(LabourCircleName, lid, total) as( select cir.FactoryCircleName, cir.FactoryCircleId lid, count(*) total from Applications app
	inner join ApplicationActions ac
		on app.AppId = ac.ApplicationRefId
	inner join UserCircleMappings ucm
		on ac.Receiver_UserRefId = ucm.UserRefId and ucm.Version=2
	inner join FactoryCircles cir
		on ucm.FactoryCircleRefId = cir.FactoryCircleId
	where app.ApplicationType = 70 and ac.AppActionType in (3,6) 
	group by cir.FactoryCircleName, cir.FactoryCircleId 
	)
	select cir.FactoryCircleName,up.FirstName +' ' + up.LastName AS 'Pending With',
	(case when c.total is null Then 0 else c.total end) AS TotalPendingApplications
	from cte c
	right join FactoryCircles cir
		on lid = cir.FactoryCircleId
	inner join UserCircleMappings ucm
		on cir.FactoryCircleId = ucm.FactoryCircleRefId
	inner join UserProfileMapping upm
		on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	inner join UserProfiles up
		on upm.UserProfileRefId = up.UserProfileId
	inner join AspNetUserRoles anr
		on upm.UserRefId = anr.UserId
		where cir.Version=2 and anr.RoleId ='692e93a4-9351-4d8a-9ed3-ecf9c3574178'
	order by cir.FactoryCircleName



-- Total Pending applications (ADF/DDF)
with cte(LabourCircleName, lid, total) as( select cir.FactoryCircleName, cir.FactoryCircleId lid, count(*) total from Applications app
	inner join ApplicationActions ac
		on app.AppId = ac.ApplicationRefId
	inner join UserCircleMappings ucm
		on ac.Receiver_UserRefId = ucm.UserRefId and ucm.Version=2
	inner join FactoryCircles cir
		on ucm.FactoryCircleRefId = cir.FactoryCircleId
	where app.ApplicationType = 70 and ac.AppActionType in (101,102) 
	group by cir.FactoryCircleName, cir.FactoryCircleId 
	)
	select cir.FactoryCircleName,up.FirstName +' ' + up.LastName AS 'Pending With',
	(case when c.total is null Then 0 else c.total end) AS TotalPendingApplications
	from cte c
	right join FactoryCircles cir
		on lid = cir.FactoryCircleId
	inner join UserCircleMappings ucm
		on cir.FactoryCircleId = ucm.FactoryCircleRefId
	inner join UserProfileMapping upm
		on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	inner join UserProfiles up
		on upm.UserProfileRefId = up.UserProfileId
	inner join AspNetUserRoles anr
		on upm.UserRefId = anr.UserId
		where cir.Version=2 and anr.RoleId IN ('5b70c7bd-b591-4300-a34c-56b4bddc9416', 'ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa')
	order by cir.FactoryCircleName


--Total Approved
with cte(LabourCircleName, lid, total) as( select cir.FactoryCircleName, cir.FactoryCircleId lid, count(*) total from Applications app
	inner join ApplicationActions ac
		on app.AppId = ac.ApplicationRefId
	inner join UserCircleMappings ucm
		on ac.Sender_UserRefId = ucm.UserRefId and ucm.Version=2
	inner join FactoryCircles cir
		on ucm.FactoryCircleRefId = cir.FactoryCircleId
	where app.ApplicationType = 70 and ac.AppActionType in (200) 
	group by cir.FactoryCircleName, cir.FactoryCircleId 
	)
	select cir.FactoryCircleName,up.FirstName +' ' + up.LastName as 'Approved By',
	(case when c.total is null Then 0 else c.total end) AS TotalApproved
	from cte c
	right join FactoryCircles cir
		on lid = cir.FactoryCircleId
	inner join UserCircleMappings ucm
		on cir.FactoryCircleId = ucm.FactoryCircleRefId
	inner join UserProfileMapping upm
		on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	inner join UserProfiles up
		on upm.UserProfileRefId = up.UserProfileId
		inner join AspNetUserRoles anr
		on upm.UserRefId = anr.UserId
		where cir.Version=2 and anr.RoleId IN ('5b70c7bd-b591-4300-a34c-56b4bddc9416', 'ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa')
	order by cir.FactoryCircleName


--Total In-Onjection
with cte(LabourCircleName, lid, total) as( select cir.FactoryCircleName, cir.FactoryCircleId lid, count(*) total 
from Applications app
	inner join ApplicationActions ac
		on app.AppId = ac.ApplicationRefId
	inner join UserCircleMappings ucm
		on ac.Sender_UserRefId = ucm.UserRefId and ucm.Version=2
	inner join FactoryCircles cir
		on ucm.FactoryCircleRefId = cir.FactoryCircleId
	where app.ApplicationType = 70 and ac.AppActionType in (404) 
	group by cir.FactoryCircleName, cir.FactoryCircleId 
	)
	select cir.FactoryCircleName,up.FirstName +' ' + up.LastName as 'Objection Raised By',
	(case when c.total is null Then 0 else c.total end) AS ObjectionRaisedInApplications
	from cte c
	right join FactoryCircles cir
		on lid = cir.FactoryCircleId
	inner join UserCircleMappings ucm
		on cir.FactoryCircleId = ucm.FactoryCircleRefId
	inner join UserProfileMapping upm
		on ucm.UserRefId = upm.UserRefId and upm.IsActive=1
	inner join UserProfiles up
		on upm.UserProfileRefId = up.UserProfileId
		inner join AspNetUserRoles anr
		on upm.UserRefId = anr.UserId
		where cir.Version=2 and anr.RoleId IN ('5b70c7bd-b591-4300-a34c-56b4bddc9416', 'ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa')
	order by cir.FactoryCircleName



	










	


	






