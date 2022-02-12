select us.UserName, aspR.Name
from AspNetUsers us join AspNetUserRoles ro
	on us.Id = ro.UserId join AspNetRoles aspR on aspR.Id = ro.RoleId