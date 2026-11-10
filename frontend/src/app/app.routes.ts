import { Routes } from '@angular/router';
import { Dashboard } from './pages/dashboard/dashboard/dashboard';
import { authGuard } from './guard/auth-guard/auth-guard';
import { adminGuard } from './guard/admin-guard/admin-guard';
import { HomeScreen } from './pages/assets/home-screen/home-screen';
import { AdminScreen } from './pages/assets/admin-screen/admin-screen/admin-screen';
import { CashFlow } from './pages/assets/cash-flow/cash-flow';
import { NLogin } from './pages/n-login/n-login';

export const routes: Routes = [

    {path: '', component: NLogin},

    {path: 'dashboard', component: Dashboard, canActivate: [authGuard]},

    {path: 'homeScreen', component: HomeScreen, canActivate: [authGuard]},

    {path: 'adminScreen', component: AdminScreen, canActivate: [adminGuard]},

    {path: 'cashScreen', component: CashFlow, canActivate: [adminGuard]},

    // {
    //     path: 'admin',
    //     canActivate: [adminGuard],
    //     children: [
    //         {
    //         path: 'users',
    //         component: AdminUsersComponent
    //         },
    //         {
    //         path: 'settings',
    //         component: AdminSettingsComponent
    //         }
    //     ]
    // }
];
