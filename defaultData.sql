insert into commands(HowTo, Line, Platform)
  values ('How to create migrations', 'dotnet ef migrations add <Name>', 'EF CORE'),
         ('How to run migrations', 'dotnet ef database update', 'EF CORE'),
         ('install npm packages', 'npm i', 'NPM'),
         ('list docker images', 'docker ps --all', 'DOCKER');