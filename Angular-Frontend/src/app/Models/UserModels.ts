export interface loginModel {
    Email: string,
    Password: string
}

export interface signUpModel {
    FullName: string,
    Email: string,
    Password: string,
    ConfirmPassword: string,
    PanNumber:string,
    BankName:string,
    BankAccountNumber: string // try using long
}