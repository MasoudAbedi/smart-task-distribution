function xN = Unnormalize_Fcn( x,MinX,MaxX )
%UNNORMALIZE_FCN Summary of this function goes here
%   Detailed explanation goes here (max - min) * z + min
xN = ((((MaxX - MinX)/2)*(x+1)))+MinX;%/2-1;

end

