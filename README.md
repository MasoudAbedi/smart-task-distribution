# smart-task-distribution
In this page, we present the source codes of the following papers:


## Case Study
Our  case  study is  based  on  a delay-sensitive application produced   by   “Rayan   Tahlil   Sepahan”   Company.   This application  is specially designed to analyze the effect of different  working  conditions  on body of  workers who  workin  environments  with  high  amounts  of  heat  stress  such  as iron and steel industry. Using the mentioned application, we can  estimate  the  sweat  rate,  change  of  water  level  and  the temperature   of   skin,  core,   vein,  and   artery   in   different segments  of  body  by  using  WBSNs  and  environmental sensors.  As  a  result,  workers  will  be  informed  about  their health status and specific  working  schedules for any worker could  be  prepared, aiming to  keep them safe  and  healthy in their working environment.

## The Training Process
We developed the following training algorith in matlba, in order to train the neural networks for predecting the size of results and the response times of the generated tasks. 

%% Start of Program
clc
clear
close all
%tic

%% Data Loading
Data = xlsread('Datawith3FogX3Run/Mehdi/70Train.csv');
%Data=Data(1:200,:);
X=Data(:,1:end-6);
Y=Data(:,end-1:end);

Data1 = xlsread('Datawith3FogX3Run/Mehdi/70Test.csv');

X1=Data1(:,1:end-6);
Y1=Data1(:,end-1:end);


DataNum = size(X,1);
InputNum = size(X,2);
OutputNum = size(Y,2);

%% Normalization
MinX = min(X);
MaxX = max(X);

MinY = min(Y);
MaxY = max(Y);

XN = X;
YN = Y;

XN1 = X1;
YN1 = Y1;

for ii = 1:InputNum
    XN(:,ii) = Normalize_Fcn1(X(:,ii),MinX(ii),MaxX(ii));
    XN1(:,ii) = Normalize_Fcn1(X1(:,ii),MinX(ii),MaxX(ii));
end


for ii = 1:OutputNum
    YN(:,ii) = Normalize_Fcn1(Y(:,ii),MinY(ii),MaxY(ii));
    YN1(:,ii) = Normalize_Fcn1(Y1(:,ii),MinY(ii),MaxY(ii));
end

%% Test and Train Data
TrPercent = 100;
TrNum = round(DataNum * TrPercent / 100);
TsNum = DataNum - TrNum;

R = randperm(DataNum);
trIndex = R(1 : TrNum);
tsIndex = R(1+TrNum : end);

Xtr = XN(trIndex,:);
Ytr = YN(trIndex,:);

Xts = XN(tsIndex,:);
Yts = YN(tsIndex,:);

%% Network Structure
pr = [-1 1];
PR = repmat(pr,InputNum,1);

Network = newff(PR,[4 OutputNum],{'tansig' 'tansig' 'tansig' 'tansig' 'purelin'});
Network.divideFcn='divideint';%divideint
Network.divideParam.trainRatio=80/100;
Network.divideParam.testRatio=10/100;
Network.divideParam.valRatio =10/100;
%Network.trainParam.max_fail=5;

%% Training
%Networknet.trainParam.showWindow = false;

tic
Network = train(Network,Xtr',Ytr');

toc
%% Assesment
%YtrNet = sim(Network,Xtr')';
YN1Net = sim(Network,XN1')';
YN1NetUN=YN1Net;
%YtrNetUN=YtrNet;
%MSEtr = mse(YtrNet - Ytr)
MSEts = mse(YN1Net - YN1)

%YS1_1 = Unnormalize_Fcn(YtrNet(:,1),MinY(1),MaxY(1));
%YS1_2 = Unnormalize_Fcn(YtrNet(:,2),MinY(2),MaxY(2));

YSY1_1 = Unnormalize_Fcn(YN1Net(:,1),MinY(1),MaxY(1));
YSY1_2 = Unnormalize_Fcn(YN1Net(:,2),MinY(2),MaxY(2));





