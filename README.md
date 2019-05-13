# PublicationHarvester
PublicationHarvester: An open-source software tool for science policy research that gathers and analyzes PubMed bibliographic data

## Overview
The Publication Harvester is a software tool that downloads publications from PubMed, stores them in a database, and generates an accurate count of publications for a set of people. The harvester uses a set of possible name variations for that individual, and records the list of authors. The goal of the software is to gather large amounts of data about specific people from PubMed for statistical analysis. It records the people, publications and publication data in a database, and generates reports based on that data.

It consists of several different components:
* PublicationHarvester: http://www.stellman-greene.com/PublicationHarvester/
* SC/Gen and SocialNetworking: http://www.stellman-greene.com/SCGen/
* FindRelated: http://www.stellman-greene.com/FindRelated/

It is also related to the ScientificDistance project: http://www.stellman-greene.com/ScientificDistance/

## Checking out and building

```
C:\git>git clone -q https://github.com/andrewstellman/PublicationHarvester/

C:\git>cd PublicationHarvester

C:\git\PublicationHarvester>msbuild /verbosity:quiet
Microsoft (R) Build Engine version 16.0.461+g6ff56ef63c for .NET Framework
Copyright (C) Microsoft Corporation. All rights reserved.

C:\git\PublicationHarvester>cd Distribution

C:\git\PublicationHarvester\Distribution>PublicationHarvester.exe
```

## Database requirements
PublicationHarvester requires [MySQL Community 5.7 (or later)](https://dev.mysql.com/downloads/mysql/). It uses [MySQL Connector/ODBC 8.0 (or later)](https://dev.mysql.com/downloads/connector/odbc/) to connect to the database. The binaries are built as x86 binaries, which means you **must run the 32-bit ODBC administrator (`odbcad32.exe`) to create the ODBC data sources**.

## License
This software is released under the GNU General Public License (GPL): http://www.gnu.org/copyleft/gpl.html

The Publication Harvester project is maintained by Andrew Stellman of Stellman & Greene Consulting. If you have questions, comments, patches, or bug reports, please contact pubharvester@stellman-greene.com.

We gratefully acknowledgement is given to the financial support of the National Science Foundation (Award SBE-0738142).

© 2019 Stellman & Greene Consulting LLC - http://www.stellman-greene.com
