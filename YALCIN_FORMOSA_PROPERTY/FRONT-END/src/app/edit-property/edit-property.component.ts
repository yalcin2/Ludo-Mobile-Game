import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IProperty } from '../property/property';
import { ApiService } from '../api.service';
import { NgForm } from '@angular/forms/src/directives/ng_form';

@Component({
  selector: 'edit-prop',
  templateUrl: './edit-property.component.html',
  styleUrls: ['./edit-property.component.css'],
  providers: [ApiService]
})
export class EditPropertyComponent 
                        implements OnInit {
  private pageTitle: String = "Update the property: ";
  @Output() updateProperty: EventEmitter<IProperty> = new EventEmitter<IProperty>();

  constructor(private _apiService: ApiService,
              private fb: FormBuilder,
              private _route: ActivatedRoute,
              private _router: Router) { }
 
  propForm: FormGroup;

  ngOnInit() {
  	this.propForm = this.fb.group({
  		 propertyType:[''],
       location:[''],
       price:[''],
       description:[''],
       imageUrl:[''],
       _id:['']
  	});
  }

  /*
  onSubmit(f: NgForm) {
    this._apiService.updateProperty(this.propForm.value).subscribe(
        response=> {
            if(response.success== true)
                this.updateProperty.emit(this.propForm.value);
    },);
    f.form.reset();
    window.location.reload();
  }
  */

  onBack(): void
  {
      this._router.navigate(['/property']);
  }
}