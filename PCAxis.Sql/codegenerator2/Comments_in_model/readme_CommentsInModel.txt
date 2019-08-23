Get XMI-file for cnmm from Sven/Sweden
Transform it with xmi2commentsInModel.xslt
save output as CommentsInModel_NN.xml

********* OLD *************
open .rtf
Fjerna table of contwents og figur
save as xml : cnmm_comments_21_step1.xml

transformer med step1_til_step2.xslt
til resultatet valider (mot cnmm_comments_step2.xsd)

og lagret det som cnmm_comments_21_step2.xml

transformerte det med step2_til_CommentsInModel.xslt  til resultatet valider (mot CommentsInModel.xsd)


og lagra det som CommentsInModel_21.xml

***

Validerings problemer:
Statistics Sweden: Not yet implemented. Virker som :et lager problemer av en eller 
annen grunn. Fjerna "linja", eller replacea "Statistics Sweden:" med "At Statistics Sweden,"

Noen felt manglet kommentar.

****
For 2.3 måtte endre schema litt siden tabelloverskriften "Table Lalala" var blitt erstattet med kun "Lalala", så det ikke var så enkelt å skille 
ut hva som var tabelloverskrift.

