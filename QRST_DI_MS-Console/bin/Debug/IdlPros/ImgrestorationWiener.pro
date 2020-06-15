pro ImgrestorationWiener,strOpen_Path,strSave_Path,strCross_Path,strAlong_Path,sharedBufferName
;  strOpen_Path='E:\IMAGE\ccdbj050526-3.tif'
;  strSave_Path='E:\abc.tif'
;  strCross_Path='E:\CrossMTF.txt'
;  strAlong_Path='E:\AlongMTF.txt'
;  sharedBufferName='ImgrestorationWiener20110324221632'

queryStatus = query_image(strOpen_Path, imageInfo)
    imageSize = imageInfo.dimensions
    Col=imageSize[0]
    Row=imageSize[1]
  arrOri_Image=findgen(Col,Row)
  arrOri_Image=read_image(strOpen_Path)
  fltOri_Image=float(arrOri_Image)


  openr, lun, strCross_Path, /get_lun
  mtfx= findgen(6)
  readf, lun, mtfx
  free_lun, lun
  openr, lun, strAlong_Path, /get_lun
  mtfy= findgen(6)
  readf, lun, mtfy
  free_lun, lun

    a=size(mtfx,/n_element)
    c=size(mtfy,/n_element)
    b=2*a-1
    fx=findgen(a)/((a-1)*2)
    fy=findgen(c)/((c-1)*2)

    i=a-1
    j=0
    mx=findgen(b)
    while i gt -1 do begin
        mx[j]=mtfx[i]
        j=j+1
        i=i-1
    endwhile   
    mx[a:b-1]=mtfx[1:a-1]
    i=a-1
    j=0
    my=findgen(b)
    while i gt -1 do begin
        my[j]=mtfy[i]
        j=j+1
        i=i-1
    endwhile 
    my[a:b-1]=mtfy[1:a-1]
    ;adjust the MTF
    admtf=fltarr(b,b)
    admtf[0:a-1,a-1]=mx[0:a-1]
    admtf[a-1,0:a-1]=my[0:a-1]
    admtf[0,0]=(mtfx[a-1]+mtfy[a-1])/2*0.9
    ray=findgen(a-1)
    rax=findgen(a-1)
    for i=2,a do begin
        ray[i-2]=(my[i-1]-my[i-2])/(my[a-1]-my[i-2])
        admtf[0,i-1]=ray[i-2]*(admtf[0,a-1]-admtf[0,i-2])+admtf[0,i-2]
        rax[i-2]=(mx[i-1]-mx[i-2])/(mx[a-1]-mx[i-2])
    endfor
    for i=1,a-1 do begin
        for j=2,a-1 do begin
            admtf[j-1,i-1]=rax[j-2]*(admtf[a-1,i-1]-admtf[j-2,i-1])+admtf[j-2,i-1]
        endfor
    endfor
    i=a-2
    j=a
    while (i gt -1)||(j lt b-2) do begin
          admtf[j,0:a-1]=admtf[i,0:a-1]
          i=i-1
          j=j+1
    endwhile
    i=a-2
    j=a
    while (i gt -1)||(j lt b-2) do begin
          admtf[0:b-1,j]=admtf[0:b-1,i]
          i=i-1
          j=j+1
    endwhile

    ;adjust the MTF
    t1=float(b)
    t2=fltarr(Row)+findgen(Row)*(t1-1)/(Row-1)+1
  t3=fltarr(Col)+findgen(Col)*(t1-1)/(Col-1)+1

    nmx=size(mx,/n_elements)
  nmy=size(my,/n_elements)
  mxx=rebin(mx,nmx,nmy)
  myy=transpose(rebin(my,nmx,nmy))

    tx1=findgen(t1,t1)
    for i=0,t1-1 do begin
        tx1[i,*]=tx1[i,0]+1
    endfor

    ty1=transpose(tx1)
    tx2=findgen(Col,Row)
    for i=0,Col-1 do begin
        tx2[i,*]=t3[i]
    endfor

    ty2=findgen(Col,Row)
    for i=0,Row-1 do begin
        ty2[*,i]=t2[i]
    endfor
    UpdateProgress,sharedBufferName,'ImgrestorationWiener','30'
    mtf=mxx*myy
    mtflr1 = MIN_CURVE_SURF(mtf, tx1, ty1, NX=Row+1, NY=Col+1,GS=[(t1-1)/(Row-1),(t1-1)/(Col-1)])
    UpdateProgress,sharedBufferName,'ImgrestorationWiener','60'
    mtflr2 = MIN_CURVE_SURF(admtf,tx1, ty1, NX=Row+1, NY=Col+1,GS=[(t1-1)/(Row-1),(t1-1)/(Col-1)])
    
  center=imageSize/2+1
  mtflr=shift(mtflr2,center)
  Nelement=n_elements(arrOri_Image)
  imgfft=fft(arrOri_Image,/double)*Nelement
  
    pu=(1/mtflr)*(mtflr^2/(mtflr^2+0.015))
  imgres3=fft(imgfft*pu,/double,/inverse)/Nelement
  imgres3=real_part(imgres3)

  ;;strImagePath=strSave_Path+'ImgrestorationWiener.tif'
  WRITE_TIFF,strSave_Path,imgres3,/APPEND     ;锟斤拷锟斤拷图锟斤拷
  ;result_message=dialog_message('图锟今保达拷锟斤拷希锟�',/information)
  UpdateProgress,sharedBufferName,'ImgrestorationWiener','100'
  vertOri_Image = REVERSE(arrOri_Image,2)
  vertPro_Image = REVERSE(imgres3,2)

end