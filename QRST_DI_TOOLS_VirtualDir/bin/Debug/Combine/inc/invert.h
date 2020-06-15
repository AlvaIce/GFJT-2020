#ifndef INVERT_H
#define INVERT_H

#define PI 3.141592654
#define min(a,b) (a)>(b)?(b):(a) 
#define max(a,b) (a)<(b)?(b):(a) 


//for LUT 
#define ANGLE_STEP  5                
#define AOD_NUM     12
#define SZA_NUM     15               
#define NUMAZIMUTHS 37
#define NUMMU       20
//#define MAX_VIEW_DIRECTION 20
#define MAX_LIST_LEN 1000     //the length of list 


//for parasol data reading
#define MAX_LINE_NUM 3240
#define MAX_COL_NUM  6480
#define ANGLE_RESOLUTION_LOW 9
#define ANGLE_RESOLUTION     18
#define PIXEL_RECORD_SIZE    738
#define MAX_VIEW_DIRECTION 16

//for IGBP
#define IGBP_RESOLUTION_INVERSE 120 //


						 
				    		 


typedef struct STRU_ONEVIEW
{
	double sza,vza,azi;  //sun zenith, view zenith, azimuth
	double sca;          //scattering angle
	double r4,r6,r8,r10; //reflectance
	double rp4,rp6,rp8;  //polarized reflectance
}stOneView;

typedef struct STRU_MULTIVIEW
{
	int  nView;    //the number of directions
	int  land_sea; //100-land, 0-water, 50-mixed
	int  cloud;    //0-clear, 100-cloudy, 50-undetermined
	int  line,col; //line number and column number of sinusoidal projection
	double lon,lat;
	stOneView mViewData[MAX_VIEW_DIRECTION];
}stMultiView;


double **f2d (int nr, int nc);
void   LoadMixedLut(char* lutPath, double** ptypeLut, int modetypeNumber, int nSingLutDim);
double PtInvert(stMultiView multiData, double** ptypeLut, int nSingLutDim, 
                int modetypeNumber, double ext[6][3], int   igbpType);
                
                
                

#endif
