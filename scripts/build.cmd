@echo off

SET VALUATOR_PATH=%~dp0..\Valuator\
SET RANK_CALCULATOR_PATH=%~dp0..\RankCalculator\
SET EVENTS_LOGGER_PATH=%~dp0..\EventsLogger\

start /d%VALUATOR_PATH% dotnet build
start /d%RANK_CALCULATOR_PATH% dotnet build
start /d%EVENTS_LOGGER_PATH% dotnet build
