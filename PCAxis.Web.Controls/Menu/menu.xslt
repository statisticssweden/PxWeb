<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

	<xsl:template match="/MenuItem">
		<div>
			<p>
				<b>
					<xsl:value-of select="prestext"/>
				</b>
				(<xsl:value-of select="description"/>)<br />
				<xsl:apply-templates select="MenuItem|headline|Link" />
			</p>
		</div>
	</xsl:template>

	<xsl:template match="MenuItem">
		<a href="?id={@selection}">
		<xsl:value-of select="prestext"/>
<!--		<xsl:if test="description != ''">
			(<xsl:value-of select="description" />)
		</xsl:if> -->
		</a>
		<br />
	</xsl:template>

	<xsl:template match="headline">
		<b>
			Headline: <xsl:value-of select="prestext" />
			<br />
		</b>
	</xsl:template>

	<xsl:template match="Link[@type='table']">
		<a href="http://www.statistikbanken.dk/{selection}">
		Länk till: <xsl:value-of select="prestext"/> 
		</a>&#160; 
		<xsl:apply-templates select="attribute[@name='size']" />&#160; 
		<xsl:apply-templates select="attribute[@name='modified']" />
		<br/>
	</xsl:template>

	 <xsl:template match="Link[@type='url']">
		  <a href="{selection}">
				Länk till: <xsl:value-of select="prestext"/>
		  </a>&#160;
		  <br/>
	 </xsl:template>

	 <xsl:template match="attribute">
		  <i><xsl:value-of select="@name"/>: <xsl:value-of select="."/></i>
	 </xsl:template>

</xsl:stylesheet>


