import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;
  constructor(private http: HttpClient) { }
  


  ngOnInit(): void {
    this.getUsers();

  }

  registerToggle(){
    this.registerMode = !this.registerMode
  }
  
  //Don't know why this one is not working
  // getUsers(){
  //   this.http.get('https://localhost:5001/api/users').subscribe(users => this.users = users);
  //   console.log(this.users)
  // }

  getUsers(){
    this.http.get('https://localhost:5001/api/users').subscribe(response =>{
    this.users = response;
    console.log(this.users);
  }, error =>{
    console.log(error);
  });
  }
  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }

}
