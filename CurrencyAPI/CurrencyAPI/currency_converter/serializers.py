from .models import Currency
from rest_framework import serializers


class CurrencySerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Currency
        fields = ['symbol', 'conversion', 'last_update',]
