import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Auth } from '../../services/authService/auth';

export const authGuard: CanActivateFn = () => {    //route, state
  
  const auth = inject(Auth);
  const router = inject(Router);

  const localuser = localStorage.getItem('user');
  const sessionUser = sessionStorage.getItem('user')

  const user = localuser || sessionUser;

  if (user){
    return true;
  }

  router.navigate(['/']);

  return false;
};
