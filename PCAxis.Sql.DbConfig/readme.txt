For versions 2.2 and newer, the SQL-config elements Codes,Keywords and Tables does not require childern. If your database uses the names from the (english) datamodel you may leave them empty.
You only have to give your deviations from the model, so if your codes for all but yes is normal but yes is YES (not Y) then you only have to add
<Codes>
<Code codeName="Yes" codeValue="YES">
</Codes>

This functionallity may be turned off  by adding an allowConfigDefault="false" attribute to the database element.

The files configstub_22.xml and configstub_23.xml contains the defaults. If you have a deviating column name you must also include the surounding table-element.


--**************************************************  
On Connection element (Sept.2013):
--
The old way still works, the new way just makes the configfile 2 lines shorter for normal cases.

The Connection element
Contained (old): ConnectionString and optionaly DefaultUser and DefaultPassword.
Containes (new): ConnectionString and optionaly KeyForUser and KeyForPassword .

The two new element:
Optional Element KeyForPassword which defaults to "Password";
Optional Element KeyForUser which defaults to "User Id"

Technically you can have all four optional elements present, but they are used in pairs, the two last will not be read if the first two are used.

The old way:
the connectionstring has been "Must contain "=PASSWORD;" and "=USER;" which is replaced by proper values, fould in DefaultUser and DefaultPassword.
(They were marked as optional in the schema, but I can't see how that would work...

The new way:
the connectionstring contains usercredentials for the default user. If the key in the connectionstring that holds the User is different from the default
of the KeyForUser element then it must be supplied. The same for password. 

A ResetConnectionString has been added, it assumes that the key for User/Password is the same.





 