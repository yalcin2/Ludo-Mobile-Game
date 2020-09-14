import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IProperty } from '../property/property';
import { ApiService } from '../api.service';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { empty } from 'rxjs/Observer';

@Component({
  selector: 'add-prop',
  templateUrl: './add-property.component.html',
  styleUrls: ['./add-property.component.css'],
  providers: [ApiService]
})
export class AddPropertyComponent 
                        implements OnInit {
  private pageTitle: String = "Add a new property: ";
  @Output() addProperty: EventEmitter<IProperty> = new EventEmitter<IProperty>();

  constructor(private _apiService: ApiService,
              private fb: FormBuilder,
              private _route: ActivatedRoute,
              private _router: Router) { }
 
  propForm: FormGroup;

  ngOnInit() {
  	this.propForm = this.fb.group({
  		  propertyType:['',[Validators.required]],
        location:['',[Validators.required]],
        price:['',[Validators.required]],
        description:['',[Validators.required]],
        imageUrl:[''],
        _id:['']
  	});
  }

  onSubmit(f: NgForm) {
    this._apiService.addProperty(this.propForm.value).subscribe(
      response=> {
          if(response.success== true)
              this.addProperty.emit(this.propForm.value);
      },);

    f.form.reset();
    window.location.reload();
  }

  onBack(): void
  {
      this._router.navigate(['/property']);
  }
}