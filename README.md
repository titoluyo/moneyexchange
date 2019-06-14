# MoneyXchange

## Prerequisites

### Software
* Git
* Node v10.16.0
* npm v6.9.0
* AngularCLI (run: `npm install -g @angular/cli`)
* .Net Core 2.2 (https://dotnet.microsoft.com/download)

### Download Source Code

   Create a work folder and within shell (Git Bash in Windows) run:  
`git clone https://github.com/titoluyo/moneyexchange.git`


### Run Backend
   Then move to folder for backend:  
`cd moneyexchange/moneyexchange/Api.Fixer`

   Restore dependencies:  
`dotnet restore`

   Execute Backend:  
`dotnet run`

   You can test the api with any Rest Api client (like Postman) or even from browser  
https://localhost:5001/latest?base=USD&symbols=EUR

### Run Frontend

   Open other shell window and then move in your work folder and then move to web source:  
`cd moneyexchange/moneyexchange-web/`

   Install dependencies:  
`npm install`

   Run the web:  
`ng serve`

   You can access the web from:  
`http://localhost:4200/`
