import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators, NgForm } from '@angular/forms';
import { ApiService } from '../api.service';
import { SignUp } from "./sign-up";


@Component({
  templateUrl: 'sign-up.component.html',
  styleUrls: ['sign-up.component.css'],
  providers: [ApiService]
})

export class SignUpComponent 
                     implements OnInit{
    
    private newList :SignUp;
    signUpForm: FormGroup;
    client: SignUp = new SignUp();

    constructor(private fb: FormBuilder,
                private _apiService: ApiService) {}
                
    @Output() addUser: EventEmitter<SignUp> = new EventEmitter<SignUp>();

    ngOnInit(): void 
    {
        this.signUpForm = this.fb.group({
            firstName:['',[Validators.required, Validators.maxLength(20), Validators.minLength(3)]],
            lastName:['',[Validators.required, Validators.maxLength(20), Validators.minLength(3)]],
            email:['',[Validators.required, Validators.pattern('[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+')]], 
            confirmEmail:['',[Validators.required, Validators.pattern('[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+')]],            
            mobile:['',[Validators.required, Validators.minLength(8), Validators.maxLength(8)]],
            catalog:[''],
            notification:['',[Validators.required]]
          });
    }

    
    onSubmit(f: NgForm) {
        this._apiService.addUser(this.signUpForm.value).subscribe(
            response=> {
                if(response.success== true)
                    this.addUser.emit(this.signUpForm.value);
            },);
        f.form.reset();
        window.location.reload();
    }
      
}
