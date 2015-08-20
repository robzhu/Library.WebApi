FROM mono

EXPOSE 5000

RUN mkdir /usr/library.webapi
ADD . /usr/library.webapi
WORKDIR /usr/library.webapi

RUN nuget restore
RUN xbuild

CMD [ "mono", "Library.WebApi/bin/Debug/Library.WebApi.exe" ]
