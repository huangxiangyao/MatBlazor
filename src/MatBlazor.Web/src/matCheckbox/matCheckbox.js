import './matCheckbox.scss';
import {MDCFormField} from '@material/form-field';
import {MDCCheckbox} from '@material/checkbox';
import {getMatBlazorInstance, setMatBlazorInstance} from '../utils/base';

export function init(ref, componentRef) {
  var self = setMatBlazorInstance(ref, {
    ref,
    componentRef,
    checkbox: null
  });

  self.checkbox = new MDCCheckbox(self.componentRef);
  let formField = new MDCFormField(self.ref);
  formField.input = self.checkbox;
}


export function setIndeterminate(ref, value, indeterminate) {
  var self = getMatBlazorInstance(ref);
  if (value == null && indeterminate) {
    self.checkbox.indeterminate = true;
  }
}
