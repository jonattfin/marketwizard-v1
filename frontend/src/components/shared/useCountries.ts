import { useState } from "react";
import { COUNTRY_CODES } from "./helpers";

export const useCountries = () => {
  const [countries, setCountries] = useState<string[]>(COUNTRY_CODES);

  const onCountryChanged = (countryCode: string, checked: boolean) => {
    if (countries.length === 1 && countries[0] === countryCode) return;

    if (checked) {
      setCountries([...countries, countryCode]);
    } else {
      setCountries(countries.filter((i) => i !== countryCode));
    }
  };

  return { countries, onCountryChanged };
};
