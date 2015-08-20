FROM mono

RUN mkdir /usr/library.webapi
ADD . /usr/library.webapi
WORKDIR /usr/library.webapi

RUN nuget restore
RUN nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory testrunner
RUN xbuild

ENTRYPOINT [ "mono", "./testrunner/NUnit.Runners.2.6.4/tools/nunit-console.exe ./Library.WebApi.UnitTests/bin/Release/Library.WebApi.UnitTests.dll" ]
