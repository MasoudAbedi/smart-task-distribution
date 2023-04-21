function xN = Normalize_Fcn1(x,MinX,MaxX)

if MinX ==MaxX
   xN =zeros(size(x));
else
xN = (x - MinX) / (MaxX - MinX)*2-1;
%end
end


