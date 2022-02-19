import { error } from '@angular/compiler/src/util';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
// import { fail } from 'assert';
// import { EventEmitter } from 'stream';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Input() usersFromHomeComponent: any;
  @Output() cancelRegister = new EventEmitter();
  model: any ={};
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];
  
  constructor(private accountService: AccountService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {

    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);

  }

  register(){
    // console.log(this.registerForm.value)
    // console.log(this.model);
    // this.accountService.register(this.model).subscribe(response=>{
    //   console.log(response)
    //   this.router.navigateByUrl('/members')
    //   this.toastr.success("You have registered successfully.") 
    //using registerForm instead of model

    this.accountService.register(this.registerForm.value).subscribe(response=>{
      console.log(response)
      this.router.navigateByUrl('/members')
      this.toastr.success("You have registered successfully.") 

    
      // this.cancel()
    }, error =>
    {
      this.validationErrors = error;     
      console.log(this.validationErrors) 
      // console.log(error);
      // this.toastr.error(error.error);
      
    })
    // this.toastr.error(error.error);

  }

  // initializeForm()
  // {
  //   this.registerForm = new FormGroup({
  //     username: new FormControl('', Validators.required),
  //     password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
  //     // confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
  //     confirmPassword: new FormControl('', [Validators.required, Validators.minLength(4)]),
  //     dateOfBirth: new FormControl(),
  //     knownAs: new FormControl(),
  //     gender: new FormControl(),
  //     introduction: new FormControl(),
  //     lookingFor: new FormControl(),
  //     interests: new FormControl(),
  //     city: new FormControl(),
  //     country: new FormControl(),

  //   })
  // }

  //Using formBuilder instead of formControl
  initializeForm()
  {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
      // confirmPassword: ['', [Validators.required, Validators.minLength(4)]],
      dateOfBirth: ['', Validators.required],
      knownAs: ['', Validators.required],
      gender: ['', Validators.required],
      // introduction: ['', Validators.required],
      // lookingFor: ['', Validators.required],
      // interests: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],

    })
  }


  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      if(control?.parent?.controls && Object.keys(control?.parent?.controls).length > 0){
        return control?.value === (control?.parent?.controls as any)[matchTo]?.value
        ? null : {isMatching: true}
      }
      
     return null;
    }
  }



  cancel(){

    
    this.cancelRegister.emit(false);
    // console.log("cancelled")
  }
  
}

