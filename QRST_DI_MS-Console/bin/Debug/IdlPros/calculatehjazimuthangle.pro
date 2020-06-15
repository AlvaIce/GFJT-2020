pro CalculateHJAzimuthAngle,file_angle,Imagepath,outtif,SharedBufferName
  outtext=strmid(outtif,0,strlen(outtif)-4)+'.txt'
  length=file_lines(file_angle)
  ok = query_tiff(ImagePath,s)
  R=s.DIMENSIONS[0]
  L=s.DIMENSIONS[1]
  head0=''
  data_angle=make_array(8,length-3,/float)
  openr,lun,file_angle,/get_lun
  readf,lun, head0
  readf,lun, head0
  readf,lun, head0
  readf,lun, data_angle
  free_lun,lun
  index=where(data_angle[0,*] eq 1,count)
  bb=(length-3)/count
  data=reform(data_angle[7,*],count,bb)
 Azimuth=congrid(data,R,L,/interp)
  UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','10'
 for j=0L,L-1,1L do begin
 image = read_tiff(ImagePath,SUB_RECT=[0,j,R,1])
 ;processbar,outtext,fix(j/(L/100.0d))
 if j eq fix((L-1)/8) then begin
 UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','20'
 endif
 if j eq fix(2*(L-1)/8) then begin
 UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','30'
 endif
 if j eq fix(3*(L-1)/8) then begin
 UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','40'
 endif
 if j eq fix(4*(L-1)/8) then begin
 UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','50'
 endif
 if j eq fix(5*(L-1)/8) then begin
 UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','60'
 endif
 if j eq fix(6*(L-1)/8) then begin
 UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','70'
 endif
  if j eq fix(7*(L-1)/8) then begin
 UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','80'
 endif
 if j eq L-1 then begin
 UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','90'
 endif
 
   for i=0L,R-1,1L do begin
    if FINITE(image[i,0]) eq 0 or image[i,0] eq 0 then begin
   Azimuth[i,j]=0
    endif
    endfor
 endfor
  WRITE_TIFF,outtif,Azimuth,COMPRESSION=0,/Float
  ;processbar,outtext,100
UpdateProgress,sharedBufferName,'CalculateHJAzimuthAngle','100'
  File_Delete,outtext
end



pro processbar,outtext,num
  openw,lun,outtext,/get_lun
  printf,lun,num
  close,/all
end
;pro CalculateHJAzimuthAngle
;  file_angle='C:\Users\zy\Desktop\Sat_Zenith_Azimuth.txt'
;  Imagepath='C:\Users\zy\Desktop\HJ1B-CCD2-451-80-20091021-L20000189148-1.tif'
;  outtif='C:\Users\zy\Desktop\Azimuth.tif'
;  CalculateHJAzimuthAngle1,file_angle,Imagepath,outtif
;end 


