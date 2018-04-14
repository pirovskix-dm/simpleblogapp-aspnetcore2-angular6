import { GlobalEventsService } from './../services/globalevents.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { UserService } from '../services/user.service';
import { map } from 'rxjs/operators';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private userService: UserService,
        private globalEventsService: GlobalEventsService
    ){
    }
    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        return this.userService.userIsSigned().pipe(
            map((isSigned: boolean) => {
                if (!isSigned) {
                    this.globalEventsService.showAdminNavBar.emit(false);
                    this.router.navigate(['/login']);
                    return false;
                }
                this.globalEventsService.showAdminNavBar.emit(true);
                return true;
            })
        );
  }
}
