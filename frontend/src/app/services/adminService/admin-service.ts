import { Injectable, Service } from '@angular/core';

// @Service()
@Injectable({
    providedIn: 'root'
})
export class AdminService {
    // getRole(): string |null {
    //     const token = localStorage.getItem('user');
    //     if (!token) return null;

    //     const payload = JSON.parse(atob(token.split('.')[1]));
    //     return payload.role;
    // }

    // isAdmin(): boolean {
    //     return this.getRole() === 'Admin';
    // }

    getUser() {
        const user = localStorage.getItem('user') || sessionStorage.getItem('user');

        if (!user) return null;

            return JSON.parse(user);
    }

    isAdmin(): boolean {
        return this.getUser()?.role === 'Admin' 
        // || this.getUser()?.role === 'Hr';
    }
}
