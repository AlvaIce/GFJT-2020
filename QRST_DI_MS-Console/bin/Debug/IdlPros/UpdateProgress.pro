Pro UpdateProgress,SharedBufferName,key,progress 
  space=' '
  exefile='IDLProgressBar.exe'
  CMD=exefile+space+SharedBufferName+space+key+','+progress
  SPAWN,CMD,/HIDE,/NOWAIT,/NOSHELL
end