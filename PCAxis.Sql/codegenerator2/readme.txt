Except for the one that creates code, files are not written to outside the folder of the program.
The programs uses input from the folder of the program. 
So some manual file moving is needed when a new XMI is received.


program                 purpose

Comments_in_model:       A xslt that transforms a newly arrived XMI to a comments_in_model.xml. 

Merge_Comments_and_oldMaster:      depending on the hardcoded version combines a newly created comments_in_model and an old_master_NN 
                       to create a new_master_NN

MakeCodeForCNMMnn:              depending on the hardcoded version   Creates code from master_NN.xml
 
