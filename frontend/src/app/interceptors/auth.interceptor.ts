import { HttpInterceptorFn } from "@angular/common/http";

export const authInterceptor : HttpInterceptorFn = (req, next) => {
    
    let user = localStorage.getItem('user')

    if(!user)[
        user = sessionStorage.getItem('user')
    ]
    
    if(user){
        const userData = JSON.parse(user)

        const token = userData.token

        if(token){
            const clonedReq = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });

            return next(clonedReq);
        }
    }

    return next(req);

}
