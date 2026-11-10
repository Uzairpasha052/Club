import {  Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginModel } from '../../models/login.model';
import { Router } from '@angular/router';
import { registerModel } from '../../models/register.model';

// @Service()
@Injectable({
    providedIn: 'root'
})
export class Auth {

    private apiUrl = environment.apiUrl;

    constructor(
        private http: HttpClient,
        private router: Router
    ){}

    login(user: LoginModel): Observable<any>{
        return this.http.post(
            `${this.apiUrl}/users/login`,
            user
        );
    }

    register(user: registerModel): Observable<any>{
        return this.http.post(
            `${this.apiUrl}/users/register`,
            user
        )
    }

     logout(): void{
        localStorage.removeItem('user')
        sessionStorage.removeItem('user')
        this.router.navigate(['/']);
    }

    // isLoggedIn: boolean = false;

    // private Users = [
    //     {
    //         company: '001',
    //         userId: 'admin',
    //         password: '1234'
    //     },
    //     {
    //         company: '002',
    //         userId: 'uzair',
    //         password: 'abcd'
    //     }
    // ];

    // constructor() {
    //     // Restore login state from localStorage
    //     this.isLoggedIn = localStorage.getItem('loggedIn') === 'true';
    // }
    // login(userId: string, password: string): boolean{

    //     const validUser = this.Users.some(user => 
    //         user.userId === userId &&
    //         user.password === password
    //     )

    //     if(validUser){
    //         this.isLoggedIn = true;
    //         localStorage.setItem('loggedIn', 'true');
    //         return true;
    //     }

    //     this.isLoggedIn = false
    //     localStorage.removeItem('loggedIn')
    //     return false        
    // }

    // logout(): void{
    //     this.isLoggedIn = false
    //     localStorage.removeItem('loggedIn')
    // }

    // getLoggedInStatus(): boolean {
    //     return this.isLoggedIn;
    // }

    // checkLogin(){
    //     return localStorage.getItem('loggedIn') === 'true';
    // }
}
