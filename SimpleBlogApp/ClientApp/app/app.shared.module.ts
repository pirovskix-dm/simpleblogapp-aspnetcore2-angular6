import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ToastyModule } from 'ng2-toasty';
import { TagInputModule } from 'ngx-chips';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { PostsComponent } from './components/posts/posts.component';
import { PostformComponent } from './components/postform/postform.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { PostviewComponent } from './components/postview/postview.component';
import { BlogComponent } from './components/blog/blog.component';
import { LoginComponent } from './components/login/login.component';
import { CategoriesComponent } from './components/categories/categories.component';
import { AboutComponent } from './components/about/about.component';

import { PostService } from './services/post.service';
import { CategoryService } from './services/category.service';
import { TagService } from './services/tag.service';
import { UserService } from './services/user.service';
import { GlobalEventsService } from './services/globalevents.service';

import { AuthGuard } from './guards/auth.guard';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        PostsComponent,
        PostformComponent,
        PaginationComponent,
        PostviewComponent,
        BlogComponent,
        LoginComponent,
        CategoriesComponent,
        AboutComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        TagInputModule,
        ReactiveFormsModule,
        ToastyModule.forRoot(),
        RouterModule.forRoot([
            { path: '', component: BlogComponent, pathMatch: 'full' },
            { path: 'tag/:id', component: BlogComponent },
            { path: 'admin', component: PostsComponent, canActivate: [AuthGuard] },
            { path: 'admin/categories', component: CategoriesComponent, canActivate: [AuthGuard] },
            { path: 'login', component: LoginComponent },
            { path: 'post/create', component: PostformComponent, canActivate: [AuthGuard] },
            { path: 'post/:id', component: PostviewComponent },
            { path: 'post/edit/:id', component: PostformComponent, canActivate: [AuthGuard] },
            { path: 'about', component: AboutComponent },
            { path: 'error', redirectTo: ''},
            { path: 'home', redirectTo: ''},
            { path: '**', redirectTo: '' }
        ])
    ],
    providers: [PostService, CategoryService, TagService, UserService, AuthGuard, GlobalEventsService]
})
export class AppModuleShared {
}
