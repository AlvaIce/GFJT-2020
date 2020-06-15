

#ifndef GEO_TIFF_H
#define GEO_TIFF_H

//#include "exports.h"
//#include "gdal_priv.h"
//#include "ogr_spatialref.h"
#define DLL_EXPORT  _declspec(dllexport)

typedef struct struct_GeoInfo
{
	double left,top;        //ground coordinate of top-left corner 
	double minlon, maxlat;  //(lon,lat) of top-left corner
	double dx,dy;           //resolution;
	int    wd,ht;           //dimension of image
	int    nband;
	int    zoneNumber;      //zone number 
	int    type;            //data type
	const char* projectRef; //
}stGeoInfo;

//read information from image using Gdal
int  PrintImageInfo(char* pszFilename);
//retrive the geo-information from the image file
int  GetGeoInformation(char* pszFilename, stGeoInfo& geoInfo);

//write single band buffer into the file
int  GdalWriteImageByte(char* savePath, unsigned char* pBuffer, int ht, int wd);

//write r,g,b chanel to the color Tiff file
int  GdalWriteImageByteColor(char* savePath, unsigned char* r, unsigned char* g, unsigned char* b, int ht, int wd);
//write r,g,b chanel to the color Tiff file with Geoinfo
int  GdalWriteImageByteColor(char* savePath, unsigned char* r, unsigned char* g, unsigned char* b, int ht, int wd, stGeoInfo geoInfo);
int  GdalWriteImage16UColor(char* savePath, 
	unsigned short* band1, 
	int ht, int wd, 
	int linestep, int colstep,
	stGeoInfo geoInfo);
int  GdalWriteImage16UColor(char* savePath, 
	unsigned short* band1, unsigned short* band2, unsigned short* band3, unsigned short* band4, 
	int ht, int wd, 
	int linestep, int colstep,
	stGeoInfo geoInfo);
int  GdalWriteImageUint(char* savePath, unsigned short* pBuffer, int ht, int wd);
int  GdalWriteJpgCopy(char* savePath, unsigned char* pBuffer, int ht, int wd);
int  GdalWriteFloatLL(char* savePath, float* pBuffer, int ht, int wd, 
	double minlon, double maxlon, double minlax, double maxlax);

void GroundToLL(double gx, double gy, double& lon, double& lat, stGeoInfo geoInfo);
void GeoImageMosaic(char** files, int nfile);
int  GeotiffMedianFilter(char* filename);

//read the band from the file of BYTE type
int  ReadGeoFileByte(char* filePath, int bandId, unsigned char** pBuffer, int& ht, int& wd);
int  ReadGeoFile16U(char* filePath, 
	int bandId, 
	unsigned short** pBuffer, 
	int line1, int line2, int linestep, 
	int col1, int col2, int colstep,
	int& ht, int& wd);


#endif

