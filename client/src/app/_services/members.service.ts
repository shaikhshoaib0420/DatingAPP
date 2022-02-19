import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, pipe } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';


// const httpOptions = {
//   headers: new HttpHeaders({ 
//     Authorization: 'Bearer' + JSON.parse(localStorage.getItem('user')!).tokekn
//   })
// }

@Injectable({
  providedIn: 'root'
})


export class MembersService {
  
 
  // baseUrl = 'https://localhost:5001/api/';

  baseUrl = environment.apiUrl;
  members: Member[] = [];
  // paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();
  constructor(private http: HttpClient) { }

  // getMembers(){
  //   if(this.members.length > 0) return of(this.members);
  //   return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
  //     map( members => {
  //       this.members = members;
  //       return members;
  //     })
  //   )
  // }

  //getMembers Using Pagination concept:

  // getMembers(page? : number, itemsPerPage? : number){
  getMembers(userParams: UserParams){  
  // let params = new HttpParams()

    // if(page !== undefined && itemsPerPage !== undefined){
    //   params = params.append('pageNumber', page.toString());
    //   params = params.append('pageSize', itemsPerPage?.toString())
    // }
    
   var params = this.getPaginatedHeaders(userParams.pageNumber, userParams.pageSize);
    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params);
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
   const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    // return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
    map(
        response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.Pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        }
      )
    );
  }

  private getPaginatedHeaders(pageNumber: number, pageSize: number){
    let params = new HttpParams()

    if(pageNumber !== undefined && pageSize !== undefined){
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize?.toString())
    }
    return params;
  }

    
  getMember(username: string){
   
    const member = this.members.find(x => x.username === username)
    if(member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
    // console.log(this.http.get<Member>(this.baseUrl + 'user/' + username))
  }
  
  updateMember(member: Member) {


    // throw new Error('Method not implemented.');
    return this.http.put(this.baseUrl + 'users', member)
      .pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  
  }
}