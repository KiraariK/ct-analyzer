# ct-analyzer

Требования к запуску проекта:
- Visual Studio 2015, .Net Framework 4.5.1+
- Установленный Matlab Compiler Runtime для Matlab R2013a

Настройки решения Visual Studio 2015:
- проект DICOMopener:
	В ссылки проекта необходимо добавить 2 библиотеки dll:
		1) DICOMWorker.dll - библиотека для чтения DICOM файлов (должна присутствовать на машине, где разворачивается проект)
		2) MWArray - библиотека, которая устанваливается вместе с Matlab Compiler Runtime для Matlab R2013a; она находится в папке MCR_FOLDER\toolbox\dotnetbuilder\bin\win32\v2.0\MWArray.dll
		3) CTImageSegmentation.dll - библиотека, появляющаяся в папке PROJECT_FOLDER\CTAnalyzer\Release после сборки проекта CTImageSegmentation в конфигурации Release. По умолчанию ссылка уже настроена, нужно только собрать проект.
