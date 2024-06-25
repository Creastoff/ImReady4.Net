%%%% ATHLETES CONFIGURATION
%You can set as many athletes as you want
%Use  ONE line for each athlete. Remove lines that are not required
%   Name = whatever name you want to be displayed by the app for each athlete
%   ID = athlete's ID from intervals.icu account
%   APIKey = athlete's APIKey from intervals.icu account

athletes =   {
                 'Christopher Davey', 'i62173', '2bje4nfg45jc04brecmueytia';
              };

%%%% SETTINGS

myTimeZone = '+01:00'; %use following format to set your Time Zone info '+HH:MM' or '-HH:MM'

yesterdayActivity = 'HighestLoad'; %use one of the following options 'Last', 'Longest', 'HighestLoad'

trainingAdviceMustBeSentToIntervals = true; % set to true if you want to overwrite the "Training Advice" wellness custom field

%%%% NEW SETTINGS
daysForShortTermTrend = 7; %enter a positive integer representing num days
daysForLongTermTrend = 60; %enter a positive integer representing num days
multiplier_X_StdDev_ForLongTermTrendRange = 0.75; %enter a positive value representing num days

showValuesInTrendCharts = true; % set to true/false if you want/don't want values in trend charts
