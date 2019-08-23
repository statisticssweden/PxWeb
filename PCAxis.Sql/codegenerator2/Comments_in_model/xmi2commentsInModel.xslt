<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:fn="http://www.w3.org/2005/xpath-functions"  
xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xmi="http://schema.omg.org/spec/XMI/2.1" >
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
   <xsl:template match="/">
      <xsl:element name="CommentsInModel">
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:\kode\assemblaBranches\Parser_db_24\PCAxis.Sql\codegenerator2\Comments_in_model\CommentsInModel.xsd</xsl:attribute>
            <xsl:element name="Tables">
		       <xsl:apply-templates select="//packagedElement[@xmi:type='uml:Class' and @name]"/>
		</xsl:element>
		</xsl:element>
	</xsl:template>
	
	<!-- table -->
	<xsl:template match="packagedElement">
	  <xsl:variable name="tabId" select="@xmi:id"/>
	   <xsl:element name="Table">
	   <xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute>
	   <xsl:apply-templates select="/xmi:XMI/xmi:Extension/elements/element[@xmi:idref = $tabId]"/>
	   <xsl:element name="Columns">
	      <xsl:apply-templates select="ownedAttribute[not(@association)]"/>
	   </xsl:element>
	   </xsl:element>
	</xsl:template>

<!--  table description-->
   <xsl:template match="element">
   <xsl:element name="Description">
<xsl:value-of select="properties/@documentation"/>   
   </xsl:element>
   </xsl:template>

<!-- column -->
 <xsl:template match="ownedAttribute">
  <xsl:variable name="tabId" select="../@xmi:id"/><!-- the colId is unique, but this speeds things up a bit -->
  <xsl:variable name="tab_pk_name" select="concat('PK_',../@name)"/>
  <xsl:variable name="col_name" select="@name"/>
 <xsl:variable name="colId" select="@xmi:id"/>
	   <xsl:element name="Column">
	   <xsl:attribute name="colname"><xsl:value-of select="@name"/></xsl:attribute>
	   
	   <xsl:attribute name="primarykey">
       <xsl:choose>
	   <xsl:when test="../ownedOperation[@name = $tab_pk_name]/ownedParameter[@name = $col_name]"><xsl:value-of select="'True'"/></xsl:when>
	  <xsl:otherwise>
	  <xsl:value-of select="'False'"/>
	</xsl:otherwise>
</xsl:choose>	   

	   </xsl:attribute>
	   <xsl:apply-templates select="/xmi:XMI/xmi:Extension/elements/element[$tabId]/attributes/attribute[@xmi:idref = $colId]"/>
	   </xsl:element>
 </xsl:template>
 
 <!-- column -->
 <xsl:template match="attribute">
 <xsl:attribute name="datatype"><xsl:value-of select="properties/@type"/></xsl:attribute>
 <xsl:attribute name="length"><xsl:value-of select="properties/@length"/></xsl:attribute>
 <xsl:attribute name="mandatory"><xsl:value-of select="replace(replace(properties/@duplicates,'0','False'),'1','True')"/></xsl:attribute>
 <!--xsl:attribute name="hasSL">
       <xsl:choose>
	   <xsl:when test="stereotype/@stereotype='SL'"><xsl:value-of select="'True'"/></xsl:when>
	  <xsl:otherwise>
	  <xsl:value-of select="'False'"/>
	</xsl:otherwise>
</xsl:choose>	
   </xsl:attribute-->
   <xsl:choose>
	   <xsl:when test="stereotype/@stereotype='SL'"><xsl:attribute name="hasSL"><xsl:value-of select="'True'"/></xsl:attribute></xsl:when>
	  <xsl:otherwise>
	</xsl:otherwise>
</xsl:choose>	
   
 <xsl:element name="Description">
 <xsl:value-of select="documentation/@value"/>   
 </xsl:element>
 
 </xsl:template>
 




</xsl:stylesheet>




