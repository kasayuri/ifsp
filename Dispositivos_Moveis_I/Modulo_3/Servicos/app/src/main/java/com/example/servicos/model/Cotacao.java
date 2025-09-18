package com.example.servicos.model;

import com.google.gson.annotations.SerializedName;

import java.util.Locale;

public class Cotacao {
    @SerializedName("price")
    private Double price;
    @SerializedName("timestamp")
    private long timestamp;

    public Double getPrice() {
        return price;
    }

    public String getTimestamp() {
        long timestampMilissegundos = timestamp * 1000L;
        java.text.SimpleDateFormat sdf = new java.text.SimpleDateFormat("dd/MM/yyyy HH:mm:ss", Locale.getDefault());
        sdf.setTimeZone(java.util.TimeZone.getTimeZone("GMT-3"));

        return sdf.format(timestampMilissegundos);
    }
}
