from .models import Currency
from rest_framework import viewsets
from .serializers import CurrencySerializer
from rest_framework.response import Response
from django.shortcuts import get_object_or_404
import json
import requests
import copy
from datetime import datetime, timezone, timedelta


class CurrencyViewSet(viewsets.ModelViewSet):
    url = "https://api.exchangeratesapi.io/latest?base=GBP"

    queryset = Currency.objects.all().order_by('-last_update')
    serializer_class = CurrencySerializer

    def list(self, request, format=None):
        try:
            content = requests.get(self.url).content
            dictionary = json.loads(content)

            for symbol, rate in dictionary["rates"].items():
                data = {
                    "conversion": rate,
                    "last_update": datetime.now().strftime("%Y-%m-%dT%H:%M:%S")
                }

                try:
                    obj =Currency.objects.get(symbol=symbol)
                    if datetime.now(timezone.utc) - obj.last_update > timedelta(minutes=1):
                        for key, value in data.items():
                            setattr(obj, key, value)
                        obj.save()

                except Currency.DoesNotExist:
                    new_values = {"symbol": symbol}
                    new_values.update(data)
                    obj =Currency(**new_values)
                    obj.save()
        except Exception:
            pass

        serializer = self.serializer_class(self.queryset, many=True)

        base_string = request.GET.get("base", None)
        if base_string:
            base_currency = get_object_or_404(self.queryset, pk=base_string)
            enriched_data = copy.deepcopy(serializer.data)
            for currency in enriched_data:
                currency["base"] = base_string
                currency["conversion"] = currency["conversion"]/base_currency.conversion
            return Response(enriched_data)
        else:
            enriched_data = copy.deepcopy(serializer.data)
            for currency in enriched_data:
                currency["base"] = "GBP"
            return Response(enriched_data)

    def retrieve(self, request, pk=None):
        currency = get_object_or_404(self.queryset, pk=pk)

        if datetime.now(timezone.utc) - currency.last_update > timedelta(minutes=1):
            try:
                params = f"&symbols={pk}"

                content = requests.get(self.url+params).content
                dictionary = json.loads(content)

                conversion = dictionary["rates"][pk]
                
                currency.conversion = conversion
                currency.last_update = datetime.now().strftime("%Y-%m-%dT%H:%M:%S")
                currency.save()
            except Exception:
                pass


        serializer = self.serializer_class(currency)

        base_string = request.GET.get("base",None)
        if base_string:
            base_currency = get_object_or_404(self.queryset, pk=base_string)
            enriched_data = copy.deepcopy(serializer.data)
            enriched_data["base"] = base_string
            enriched_data["conversion"] = enriched_data["conversion"]/base_currency.conversion
            return Response(enriched_data)
        else:
            enriched_data = copy.deepcopy(serializer.data)
            enriched_data["base"] = "GBP"
            return Response(enriched_data)

    